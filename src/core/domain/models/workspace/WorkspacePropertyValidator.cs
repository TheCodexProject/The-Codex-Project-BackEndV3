﻿using domain.exceptions;
using domain.models.user;
using OperationResult;

namespace domain.models.workspace;

/// <summary>
/// This class is responsible for validating the properties of a <see cref="Workspace"/>.
/// </summary>
public static class WorkspacePropertyValidator
{
    /// <summary>
    /// Validates the title of a workspace.
    /// </summary>
    /// <param name="title">Title to be checked.</param>
    /// <returns></returns>
    public static Result<string> ValidateTitle(string title)
    {
        // ? Is the title null or empty?
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result<string>.Failure(new InvalidArgumentException("Workspace title cannot be empty, please provide a title."));
        }

        return title.Length switch
        {
            < 3 => Result<string>.Failure(new InvalidArgumentException("Workspace title is too short, please provide a title with at least 3 characters.")),
            > 100 => Result<string>.Failure(new InvalidArgumentException("Workspace title is too long, please provide a title with at most 100 characters.")),
            _ => Result<string>.Success(title)
        };
    }

    /// <summary>
    /// Validates the addition of a contact to a workspace.
    /// </summary>
    /// <param name="contact"></param>
    /// <param name="contacts"></param>
    /// <returns></returns>
    public static Result ValidateAddContact(User? contact, List<User> contacts)
    {
        // ? Is the contact null or empty?
        if (contact == null)
            return Result.Failure(new InvalidArgumentException("The provided contact is invalid. Contact cannot be null."));

        // ? Does the contact already exist in the list?
        return contacts.Contains(contact) ?
            Result.Failure(new InvalidArgumentException("The provided contact already exists in the list."))
            : Result.Success();
    }

    /// <summary>
    /// Validates the removal of a contact from a workspace.
    /// </summary>
    /// <param name="contact"></param>
    /// <param name="contacts"></param>
    /// <returns></returns>
    public static Result ValidateRemoveContact(User? contact, List<User> contacts)
    {
        // ? Is the contact null or empty?
        if (contact == null)
            return Result.Failure(new InvalidArgumentException("The provided contact is invalid. Contact cannot be null."));

        // ? Does the contact exist in the list?
        return contacts.Contains(contact) ?
            Result.Success()
            : Result.Failure(new InvalidArgumentException("The provided contact does not exist in the list."));
    }

    // TODO: TO BE EXTENDED
}