using Unity.Entities;

public static class MapInitializer
{
    private static Entity lastLevel;
    public static void Initialize()
    {
        MapHolder.MapPrefabDict.TryGetValue("test_level_1", out Entity entity);
        World.DefaultGameObjectInjectionWorld.EntityManager.Instantiate(entity);
        lastLevel = entity;
    }

    public static void NextLevel()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityManager.CompleteAllJobs();
        entityManager.DestroyEntity(lastLevel);
        MapHolder.MapPrefabDict.TryGetValue("test_level_2", out Entity entity);
        lastLevel = entityManager.Instantiate(entity);
    }

   //TODO Set to specific level

   public static void MenuLevel()
   {
       
   }
}