namespace AlgoApp.Data
{
    public class StudentToClass
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public ClassRoom ClassRoom { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
    }
}
