using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using MightyGm2.Engine.Database;

namespace MightyGm2.Engine.Control
{
	/// <summary>
	/// Class for managing the DB Resources.
	///	It preloads the DBs and the Tags.
	/// </summary>
	public class DatabaseCtrl
	{
        public delegate void TagAdded(params Tag[] newTags);
        public delegate void TagRemoved(params Tag[] removedTags);
        public delegate void TagChangedName(params Tag[] changedTags);
        public event TagAdded OnTagAdded;
        public event TagRemoved OnTagRemoved;
        public event TagChangedName OnTagChangedName;

		public MightyDb DB { get; }

		// The Types filters of the research
		public bool GetMisc { get; set; }
		public bool GetPicture { get; set; }
		public bool GetSound { get; set; }
		public bool GetText { get; set; }
		public bool GetPdf { get; set; }
		public bool GetVideo { get; set; }
		public bool GetArchive { get; set; }

        public DatabaseCtrl()
		{
			DB = new MightyDb();
		}

		#region DB management
		/// <summary>
		/// Check if the given directory is already contained by at least one of the DB,
		/// or if already one of the DB.
		/// </summary>
		/// <param name="testedDir">The direcotry to check.</param>
		/// <returns>True is contained or equal to another DB.</returns>
		public bool IsContainedByAlreadyExistingDB(DirectoryInfo testedDir)
		{
			foreach (ResourceFolder db in DB.Folders)
			{
				if (testedDir.FullName.StartsWith(db.Info.FullName))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Check if the given directory already contains at least one of the DB,
		/// or if already one of the DB.
		/// </summary>
		/// <param name="testedDir">The direcotry to check.</param>
		/// <returns>True is contained or equal to another DB.</returns>
		public bool IsContainingAnAlreadyExistingDB(DirectoryInfo testedDir)
		{
			foreach (ResourceFolder db in DB.Folders)
			{
				if (db.Info.FullName.StartsWith(testedDir.FullName))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// use the File extension to determine the appropiate resource type.
		/// </summary>
		/// <param name="file">The file to check.</param>
		/// <returns>The ResourceType of the given File.</returns>
		public ResourceFileType GetResourceType(FileInfo file)
		{
			ResourceFileType ret;
			if (ApplicationControl.Control.File.IsCompatibleImage(file.Extension))
			{
				ret = ResourceFileType.Picture;
			}
			else if (ApplicationControl.Control.File.IsCompatibleSoundtrack(file.Extension))
			{
				ret = ResourceFileType.Soundtrack;
			}
			else if (ApplicationControl.Control.File.IsCompatibleText(file.Extension))
			{
				ret = ResourceFileType.Text;
			}
			else if (ApplicationControl.Control.File.IsCompatibleArchive(file.Extension))
			{
				ret = ResourceFileType.Archive;
			}
			else if (ApplicationControl.Control.File.IsCompatiblePdf(file.Extension))
			{
				ret = ResourceFileType.Pdf;
			}
			else if (ApplicationControl.Control.File.IsCompatibleVideo(file.Extension))
			{
				ret = ResourceFileType.Video;
			}
			else
			{
				ret = ResourceFileType.Misc;
			}
			return ret;
		}

		/// <summary>
		/// Checks if the resource already exists in the DB.
		/// </summary>
		/// <param name="item">The file to check.</param>
		/// <returns>True if already exists.</returns>
		public bool ResourceAlreadyInDB(FileInfo item)
		{
			var q = from t in DB.Files
					where t.Name == item.Name
					select t;
			//TODO true? The 'toArray' here is necessary because linq can't interpret the 'Info' property;
			return q.ToArray().Where(t => t.Info.FullName == item.FullName).Count() >= 1;
		}

        public IEnumerable<ResourceFile> GetResearch(IEnumerable<string> filterText, IEnumerable<int> includeTags, IEnumerable<int> excludeTags)
        {
            foreach (ResourceFile file in GetResources())
            {
                bool include = true;

                // Filter with basic text search
                foreach (string textSearch in filterText)
                {
                    bool exclude = textSearch.StartsWith("-");
                    string motif = exclude ? textSearch.Remove(0, 1) : textSearch;

                    try
                    {
                        Regex reg = new Regex(motif, RegexOptions.IgnoreCase);
                        bool containsMotif = reg.IsMatch(file.Name);
                        // Include if name contains research text
                        // Include if name does not contain excluded text
                        include = (!exclude && containsMotif) || (exclude && !containsMotif);
                    }
                    catch (Exception)
                    {
                        // Assume the regex is incomplete : dont include
                        include = false;
                    }
                    if (!include) break;
                }

                // Filter with including Tags
                var fileTags = file.ResourceFilesToTags.Select(rft => rft.Tag.Id);
                if (include && includeTags.Count() > 0)
                {
                    foreach (var tagId in includeTags)
                    {
                        if (!fileTags.Contains(tagId))
                        {
                            // Include if is taged with a researched tag.
                            include = false;
                            break;
                        }
                    }
                }

                // Filter with excluding tags
                if (include && excludeTags.Count() > 0)
                {
                    foreach (var tagId in excludeTags)
                    {
                        if (fileTags.Contains(tagId))
                        {
                            // Include if is not taged with a excluded tag.
                            include = false;
                            break;
                        }
                    }
                }

                // Include the Resource
                if (include)
                {
                    yield return file;
                }
            }

        }

        internal IEnumerable<Tag> FindTagSuggestion(string potentialTag)
        {
            potentialTag = potentialTag.ToLower();
            var result = from tag in DB.Tags
                         where tag.Name.ToLower().Contains(potentialTag)
                         orderby tag.Id
                         select tag;
            return result;
        }
        #endregion

        #region Import management
        /// <summary>
        /// Create a first draw of a new DB import.
        /// Have to be saved afterward, after possible user modifications.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public DatabaseImportResult StartImport(DirectoryInfo directory)
		{
			DatabaseImportResult result = new DatabaseImportResult();

			// Checks if the Given Folder can be used as a MightyDB.
			if (!directory.Exists)
			{
				result.ErrorMessage = "Folder does not exist";
				result.IsFolderEligible = false;
			}
			else if (IsContainedByAlreadyExistingDB(directory))
			{
				result.ErrorMessage = "Folder is already in Database";
				result.IsFolderEligible = false;
			}
			else if (IsContainingAnAlreadyExistingDB(directory))
			{
				/*
				* TODO : 'just' have to change the previous one for the new one
				*/
				result.ErrorMessage = "Folder already contains a Database. (Behavior may change in the futur)";
				result.IsFolderEligible = false;
			}
			else
			{
				result.IsFolderEligible = true;
			}

			if (!result.IsFolderEligible) { return result; }

			//Normalize name
			if (directory.FullName.EndsWith("\\"))
				directory = new DirectoryInfo(directory.FullName.TrimEnd('\\'));

            // Create DB
            ResourceFolder db = new ResourceFolder()
            {
                Name = directory.Name,
                Path = directory.FullName,
                IsActive = true
			};
			result.Database = db;

			int dirNameLength = directory.FullName.Length + 1/*Slash*/;

			// Parse every file in the directory
			foreach (var file in directory.EnumerateFiles("*", SearchOption.AllDirectories))
			{
				// Instanciate with adequate 'ResourceType'
				ResourceFile resource = new ResourceFile()
				{
					Name = file.Name,
					Type = GetResourceType(file),
					Database = db,
					RelativePath = file.FullName.Remove(0, dirNameLength),
				};

				// Tag gestion
				if (file.Directory.FullName != directory.FullName)
				{
					// Get every relative folders
					string relPathFolders = file.Directory.FullName.Remove(0, dirNameLength);
					foreach (var repName in relPathFolders.Split('\\'))
					{
						// Find associated if already created
						Tag newTag = result.Tags.FirstOrDefault(t => t.Name == repName);

						// Or create a new one
						if (newTag == null)
						{
							newTag = new Tag() { Name = repName };
							result.AddTag(newTag);
						}

						// Assign to resource
						resource.AddTag(newTag);
					}
				}

				// Add to result list
				result.AddResource(resource);
			}

			return result;
		}

		/// <summary>
		/// Save a database import result in DB, and as such, finalize the import.
		/// </summary>
		/// <param name="data"></param>
		public void Save(DatabaseImportResult data)
		{
            List<Tag> addedTags = new List<Tag>();

			foreach(TagImportation t in data.TagImportations)
			{
				if (t.toImport)
				{
					Tag existingTag = DB.Tags.FirstOrDefault(tag => tag.Name == t.tag.Name);
					if(existingTag == null)
					{
						DB.Tags.Add(t.tag);
                        addedTags.Add(t.tag);
                    }
					else
					{
						foreach (var file in data.Resources)
						{
							file.ReplaceTagIfFound(t.tag, existingTag);
						}
					}
				}
				else
				{
					foreach(var file in data.Resources)
					{
						file.RemoveTagIfFound(t.tag);
					}
				}
			}
			DB.Folders.Add(data.Database);
			DB.Files.AddRange(data.Resources);
            
			DB.SaveChanges();

            OnTagAdded?.Invoke(addedTags.ToArray());
        }

        /// <summary>
        /// Find new files to add in DB
        /// and deleted files to remove.
        /// </summary>
        /// <param name="resourceFolder">The DB to scan and update.</param>
        public void RefreshDb(ResourceFolder resourceFolder)
        {
            List<ResourceFile> toAdd = new List<ResourceFile>();
            List<ResourceFile> toRemove = new List<ResourceFile>();

            DirectoryInfo directory = resourceFolder.Info;
            int dirNameLength = directory.FullName.Length + 1/*Slash*/;

            // Parse every file in the directory and check if already imported
            foreach (var file in directory.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                string relPath = file.FullName.Remove(0, dirNameLength);
                
                if (DB.Files.FirstOrDefault(ff=>ff.RelativePath == relPath) == null)
                {
                    // Instanciate with adequate 'ResourceType'
                    ResourceFile resource = new ResourceFile()
                    {
                        Name = file.Name,
                        Type = GetResourceType(file),
                        Database = resourceFolder,
                        RelativePath = relPath
                    };
                    toAdd.Add(resource);
                }
            }

            // check every resource in DB, and check if still exists
            var dbFiles = from dbf in DB.Files
                          where dbf.DatabaseId == resourceFolder.Id
                          select dbf;
            foreach (var dbf in dbFiles)
            {
                if (!dbf.Info.Exists)
                {
                    toRemove.Add(dbf);
                }
            }

            // Update DB
            DB.AddRange(toAdd);
            DB.RemoveRange(toRemove);
            DB.SaveChanges();
        }

        /// <summary>
        /// Remove the given DB from the database.
        /// </summary>
        /// <param name="db"></param>
        public void RemoveDb(ResourceFolder db)
		{
			DB.Folders.Remove(db);
			DB.SaveChanges();
		}
		#endregion

		#region Search management
		/// <summary>
		/// Get the resources filtered per the property setted criterias.
		/// </summary>
		/// <returns>Search result.</returns>
		public IEnumerable<ResourceFile> GetResources()
		{
            var res = from t in DB.Files
                      where t.Database.IsActive
                      where GetPicture && t.Type == ResourceFileType.Picture
                      || GetSound && t.Type == ResourceFileType.Soundtrack
                      || GetText && t.Type == ResourceFileType.Text
                      || GetPdf && t.Type == ResourceFileType.Pdf
                      || GetVideo && t.Type == ResourceFileType.Video
                      || GetArchive && t.Type == ResourceFileType.Archive
                      || GetMisc && t.Type == ResourceFileType.Misc
                      orderby t.Id
                      select t;

            return res.Include(t => t.ResourceFilesToTags)
                    .ThenInclude(frt => frt.Tag)
                    .Include(t => t.Database);
        }
        #endregion

        /// <summary>
        /// Add tags to the database.
        /// </summary>
        /// <param name="newTag">The new tags</param>
        public void AddTags(params Tag[] newTag)
        {
            DB.Tags.AddRange(newTag);
            DB.SaveChanges();
            OnTagAdded?.Invoke(newTag);
        }

        /// <summary>
        /// Add tags to the database.
        /// </summary>
        /// <param name="newTag">The new tags</param>
        public void RemoveTags(params Tag[] removedTag)
        {
            DB.Tags.RemoveRange(removedTag);
            DB.SaveChanges();
            OnTagAdded?.Invoke(removedTag);
        }

        /// <summary>
        /// Change the name of the given tag in the database.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="inputText"></param>
        public void ChangeTagName(Tag tag, string inputText)
        {
            tag.Name = inputText;
            DB.SaveChanges();
            OnTagChangedName?.Invoke(tag);
        }

        /// <summary>
        /// Merge tags into one.
        /// </summary>
        /// <param name="toMerge">The tags to replace and remove.</param>
        /// <param name="replacementTag">The replacement tag.</param>
        public void MergeTags(Tag[] toMerge, Tag replacementTag)
        {
            HashSet<int> tagedFiles = new HashSet<int>();

            // Find files tagged by removed tags
            foreach (var delTag in toMerge)
            {
                foreach (var item in DB.FilesToTags.Where(ft => ft.TagId == delTag.Id))
                {
                    if (!tagedFiles.Contains(item.FileId))
                        tagedFiles.Add(item.FileId);
                }
            }

            // Add merged tag to these files
            foreach (var fileId in tagedFiles)
            {
                ResourceFile rf = DB.Files.Find(fileId);
                rf.AddTagIfNotAlreadyHave(replacementTag);
            }

            // Remove old tags and save
            DB.Tags.RemoveRange(toMerge);
            DB.SaveChanges();
            OnTagRemoved?.Invoke(toMerge);
        }
    }
}
