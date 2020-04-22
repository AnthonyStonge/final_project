using Unity.Entities;

public static class GlobalEvents
{
    public static void LockUserInputs()
    {
        //Get input components / Player entity
        EntityManager e = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = GameVariables.Player.Entity;
        
        //Set disable
        InputComponent inputs = e.GetComponentData<InputComponent>(entity);
        inputs.Enabled = false;
        e.SetComponentData(entity, inputs);
    }

    public static void LockUserInputs(ref InputComponent inputs)
    {
        inputs.Enabled = false;
    }

    public static void UnlockUserInputs()
    {
        //Get input components / Player entity
        EntityManager e = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = GameVariables.Player.Entity;
        
        //Set enable
        InputComponent inputs = e.GetComponentData<InputComponent>(entity);
        inputs.Enabled = true;
        e.SetComponentData(entity, inputs);
    }
    
    public static void UnlockUserInputs(ref InputComponent inputs)
    {
        inputs.Enabled = true;
    }
}
