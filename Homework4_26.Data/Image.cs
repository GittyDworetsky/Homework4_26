﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework4_26.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageSource { get; set; }
        public string ImageName { get; set; }
        public int Likes { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
