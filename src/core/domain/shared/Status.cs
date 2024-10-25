namespace domain.shared;

/// <summary>
/// Status for work items and projects.
/// </summary>
public enum Status
{
    None,
    Open,
    InProgress,
    ReadyForReview,
    Done,
    Closed
}