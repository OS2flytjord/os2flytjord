using Niras.Jordflytning.Core.Models;
using Niras.Jordflytning.Extensions;
using System;
using System.IO;

namespace Niras.Jordflytning.Areas.Backend.ViewModels
{
    public class PersonFileModel
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid PersonId { get; set; }
        public string PersonName { get; set; }


        public static PersonFileModel Create(FileInfo fileInfo, Person person)
        {
            return new PersonFileModel { 
                Name = fileInfo.Name, 
                FullName = fileInfo.FullName, 
                CreationTime = fileInfo.CreationTime,
                PersonId = person != null ? person.Id : Guid.Empty,
                PersonName = person.FullName()
            };
        }
    }
}