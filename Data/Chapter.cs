using System.Collections.Generic;

namespace AlgoApp.Data
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<Question> Questions { get; }
    }
}
