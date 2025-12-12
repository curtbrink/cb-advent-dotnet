namespace AdventBase.Utils;

public record BidirectionalTuple<T>(T A, T B) where T : IEquatable<T>
{
    // ensure bidirectionality in Equals and HashCode
    public virtual bool Equals(BidirectionalTuple<T>? other)
    {
        if (other is null) return false;
        return (A.Equals(other.A) && B.Equals(other.B)) || (B.Equals(other.A) && A.Equals(other.B));
    }

    public override int GetHashCode() => 16269053 * A.GetHashCode() + 16269053 * B.GetHashCode();

    public override string ToString() => $"[ {A} {B} ]";
}