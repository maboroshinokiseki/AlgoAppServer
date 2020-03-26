using AlgoApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoApp.Areas.Api.Models
{
    public class ClassRoomListModel : CommonResultModel
    {
        public List<ClassRoomModel> ClassRooms { get; set; }
    }
}
