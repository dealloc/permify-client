using Permify.Client.Exceptions;
using Permify.Client.Models.V1.Permission;

namespace Permify.Client.Contracts.V1;

/// <summary>
/// Allows checking access control and lookup up permissions for entities and subjects.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Allows checking if a given entity can perform an operation on a subject.
    ///
    /// Checks take the form of <c>Can user U perform action Y in resource Z?</c>
    ///
    /// Permify defines two main access checks:
    /// <ul>
    ///     <li>Resource Based Access Check (Relationships)</li>
    ///     <li>Attribute Based Access Check With Context Data</li>
    /// </ul>
    ///
    /// Resource Based Access Check takes the entity, operation and subject and makes a decision based on permissions granted.
    /// For an example, see <a href="https://docs.permify.co/api-reference/permission/check-api#resource-based-check-relationships">Permify's docs</a>
    ///
    /// Attribute Based Check With Context Data (ABAC) allows passing in additional context to make a decision.
    /// For an example, see <a href="https://docs.permify.co/api-reference/permission/check-api#attribute-based-abac-check-with-context-data">Permify's docs</a>
    ///
    /// Access decisions are evaluated by stored relational tuples and your authorization model, Permify Schema.
    ///
    /// In high level, access of an subject related with the relationships or attributes created between the subject and the resource.
    /// You can define this data in Permify Schema then create and store them as relational tuples and attributes,
    /// which is basically forms your authorization data.
    ///
    /// Permify Engine to compute access decision in 2 steps,
    /// <ol>
    ///     <li>Looking up authorization model for finding the given action’s ( edit, push, delete etc.) relations.</li>
    ///     <li>Walk over a graph of each relation to find whether given subject ( user or user set ) is related with the action.</li>
    /// </ol>
    /// </summary>
    /// <param name="request">The <see cref="CheckAccessControlRequest" /> to send.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> that can be used to cancel the async operation.</param>
    /// <exception cref="PermifyClientException">If Permify returns an error while attempting to check access.</exception>
    /// <seealso href="https://docs.permify.co/api-reference/permission/check-api#how-access-decisions-evaluated" />
    Task<CheckAccessControlResponse> CheckAccessControlAsync(
        CheckAccessControlRequest request,
        CancellationToken cancellationToken = default
    );
}