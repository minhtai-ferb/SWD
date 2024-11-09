﻿using CursusJapaneseLearningPlatform.Repository.Bases.BaseEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Repository.Entities
{
    public class Collection : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CollectionName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public User User { get; set; }
        public ICollection<Flashcard> Flashcards { get; set; }


        public DateTimeOffset CreatedTime { get ; set ; }
        public DateTimeOffset LastUpdatedTime { get ; set ; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string CreatedBy { get ; set ; }
        public string LastUpdatedBy { get ; set ; }
        public string? DeletedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
