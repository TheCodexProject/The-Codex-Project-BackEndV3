namespace domain.shared;

/// <summary>
/// Status for work items and projects.
/// </summary>
public enum Status
{
    None = 0,
    Open = 1,
    InProgress = 2,
    ReadyForReview = 3,
    Done = 4,
    Closed = 5
}