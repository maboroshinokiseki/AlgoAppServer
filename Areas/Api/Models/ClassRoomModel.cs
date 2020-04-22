using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Models
{
    public class ClassroomModel : CommonResultModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentCount { get; set; }
        public UserModel Teacher { get; set; }
        public List<UserModel> Students { get; set; }
    }
}
