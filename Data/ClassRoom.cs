using System.Collections.Generic;

namespace AlgoApp.Data
{
    public class ClassRoom
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public User Teacher { get; set; }
        public string ClassName { get; set; }
        public List<StudentToClass> Students { get; set; }
    }
}
