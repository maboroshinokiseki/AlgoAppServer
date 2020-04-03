using System.Collections.Generic;

namespace AlgoApp.Areas.Api.Models
{
    public class CommonListResultModel<T> : CommonResultModel
    {
        public List<T> Items { get; set; }
    }
}
