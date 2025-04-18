﻿using MongoDB.Bson;
using System;

namespace MinRobot.Infrastructure.Serialization;

public static class GuidObjectIdConverter
{
    public static ObjectId GetMatchingObjectId(string input)
    {
        if (ObjectId.TryParse(input, out var objectId))
        {
            return objectId;
        }
        else
        {
            throw new ArgumentException("The provided input is not a valid ObjectId.");
        }
    }
}