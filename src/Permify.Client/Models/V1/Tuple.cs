namespace Permify.Client.Models.V1;

/// <summary>
/// In Permify, relationships between your entities, objects, and users builds up a collection of access control lists (ACLs).
///
/// These ACLs are called relational tuples: the underlying data form that represents object-to-object and object-to-subject relations.
/// Each relational tuple represents an action that a specific user or user set can do on a resource and takes form of
/// <c>user U has relation R to object O</c>, where user U could be a simple user or a user set such as team X members.
/// </summary>
/// <example>
/// In Permify, the simplest form of relational tuple structured as: <c>entity # relation @ user</c>.
/// Here are some relational tuples with semantics:
/// <img src="https://user-images.githubusercontent.com/34595361/183959294-149fcbb9-7f10-4c1e-8d66-20a839893909.png" alt="Permify tuple example code" />
/// </example>
/// <param name="Entity">The entity that the relation is applied to.</param>
/// <param name="Relation">The relation that the entity has to the subject.</param>
/// <param name="Subject">The subject that the relation is applied to.</param>
public record Tuple(
    Entity Entity,
    string Relation,
    Entity Subject
);