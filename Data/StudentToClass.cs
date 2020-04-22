namespace AlgoApp.Data
{
    public class StudentToClass
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
    }
}
