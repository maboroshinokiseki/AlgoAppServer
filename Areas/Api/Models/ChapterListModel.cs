using AlgoApp.Data;
using System.Collections.Generic;

namespace AlgoApp.Areas.Api.Models
{
    public class ChapterListModel : CommonResultModel
    {
        public List<Chapter> Chapters { get; set; }
    }
}
