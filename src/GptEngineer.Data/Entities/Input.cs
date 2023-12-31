﻿namespace GptEngineer.Data.Entities;

using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class Input
{
    public string? Role { get; set; }
    public string? Content { get; set; }
}