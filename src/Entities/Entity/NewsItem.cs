using System;
using System.ComponentModel.DataAnnotations;

namespace YNewsApi.Entities.Entity
{
    public class NewsItem
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string By { get; set; }

        [StringLength(100)]
        public string Type { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        [StringLength(400)]
        public string Url { get; set; }


        public long Time { get; set; }

        public bool Dead { get; set; }
    }
}
