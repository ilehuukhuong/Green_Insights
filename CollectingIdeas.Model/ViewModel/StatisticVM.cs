﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.Models.ViewModel
{
    public class StatisticVM
    {
        public int Total_Ideas_without_comment { get; set; }
        public IEnumerable<Idea> List_Ideas_without_comment { get; set; }

    }
}