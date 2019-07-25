using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace App2.Core.BL
{
    public static class TaskManager
    {
        static readonly string rootPathA = @"Core/TaskManager/Tasks";
        public static async Task<List<TaskOpj>> AllAsync(string contentRootPath)
        {
            var fold = Path.Combine(contentRootPath, rootPathA);
            System.IO.Directory.CreateDirectory(fold);
            var dirInfo = new DirectoryInfo(fold);
            var files = dirInfo.GetFiles();
            var list = new List<TaskOpj>();
            foreach (var file in files)
            {
                var filetxt = await File.ReadAllTextAsync(file.FullName);
                var item = JsonConvert.DeserializeObject<TaskOpj>(filetxt);
                list.Add(item);
            }
            return list;
        }

        public static async Task<TaskOpj> createAsync(string contentRootPath, TaskOpj mod)
        {
            var fold = Path.Combine(contentRootPath, rootPathA);
            System.IO.Directory.CreateDirectory(fold);
            var date = DateTime.UtcNow;
            var opj = new TaskOpj();
            opj.guid = Guid.NewGuid().ToString();
            opj.Note = mod.Note;
            opj.createdAt = date;
            opj.modfiledAt = date;
            var file_path = Path.Combine(fold, opj.guid + ".json");
            var json = JsonConvert.SerializeObject(opj, Formatting.Indented);
            await System.IO.File.WriteAllTextAsync(file_path, json);
            return opj;
        }

        public static async Task<TaskOpj> getAsync(string contentRootPath, string guid)
        {
            var fold = Path.Combine(contentRootPath, rootPathA);
            System.IO.Directory.CreateDirectory(fold);
            var filePath = Path.Combine(fold, guid + ".json");
            if (!System.IO.File.Exists(filePath)) return null;
            try
            {
                var filetxt = await File.ReadAllTextAsync(filePath);
                var item = JsonConvert.DeserializeObject<TaskOpj>(filetxt);
                return item;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public static async Task<bool> updateAsync(string contentRootPath, TaskOpj mod)
        {
            try
            {
                var opj = await getAsync(contentRootPath, mod.guid);
                if (opj == null) return false;
                var fold = Path.Combine(contentRootPath, rootPathA);
                var date = DateTime.UtcNow;
                opj.Note = mod.Note;
                opj.modfiledAt = date;
                var file_path = Path.Combine(fold, opj.guid + ".json");
                var json = JsonConvert.SerializeObject(opj, Formatting.Indented);
                await System.IO.File.WriteAllTextAsync(file_path, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool delete(string contentRootPath, string guid)
        {
            var fold = Path.Combine(contentRootPath, rootPathA);
            System.IO.Directory.CreateDirectory(fold);
            var filePath = Path.Combine(fold, guid + ".json");
            if (!File.Exists(filePath)) return true;
            try
            {
                System.IO.File.Delete(filePath);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }


    }


    public class TaskOpj
    {
        public string guid { get; set; }
        public string Note { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? modfiledAt { get; set; }
    }
}