namespace AlgoApp.Areas.Api.Models
{
    public enum Codes
    {
        Unknown = -1,
        None = 0,
        LoginFailed,
        RegistrationFailed,
        TimeOut,
        RecordExists,
        NoRecord,
        QuestionInBookmark,
        QuestionNotInBookmark,
        NoMoreQuestions,
    }
}