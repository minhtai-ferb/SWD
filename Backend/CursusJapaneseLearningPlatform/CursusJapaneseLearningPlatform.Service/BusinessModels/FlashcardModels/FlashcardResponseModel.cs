﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Service.BusinessModels.FlashcardModels
{
    public class FlashcardResponseModel
    {
        public Guid Id { get; set; }
        public Guid CollectionId { get; set; }
        public string Word { get; set; }
        public string Meaning { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
    }
}