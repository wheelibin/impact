using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Entities;

namespace Impact.Game.Extensions
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Returns the bounding box which includes all the bricks in the supplied list
        /// </summary>
        public static CCRect GetGroupedBrickBounds(this List<Brick> bricks)
        {
            CCRect groupedBrick;

            if (bricks.Count == 1)
            {
                groupedBrick = bricks[0].BoundingBoxTransformedToWorld;
            }
            else
            {
                groupedBrick = bricks[0].BoundingBoxTransformedToWorld;
                for (int b = 1; b < bricks.Count; b++)
                {
                    groupedBrick = CCRect.Union(groupedBrick, bricks[b].BoundingBoxTransformedToWorld);
                }
            }

            return groupedBrick;
        }

        /// <summary>
        /// Returns a list of bricks that have been hit by the specified entity
        /// </summary>
        public static List<Brick> BricksHitByEntity(this CCNode entity, List<Brick> bricks)
        {
            List<Brick> bricksHitByEntity = new List<Brick>();
            foreach (Brick brick in bricks)
            {
                if (entity.BoundingBoxTransformedToWorld.IntersectsRect(brick.BoundingBoxTransformedToWorld))
                {
                    bricksHitByEntity.Add(brick);
                }
            }

            return bricksHitByEntity;

        }

    }
}
