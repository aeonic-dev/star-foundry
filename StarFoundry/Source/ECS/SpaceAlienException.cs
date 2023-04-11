using System;

namespace StarFoundry.ECS;

public class SpaceAlienException : ArgumentException {
    public SpaceAlienException(string paramName) : base("Entity belongs to a different universe!", paramName) { }
}