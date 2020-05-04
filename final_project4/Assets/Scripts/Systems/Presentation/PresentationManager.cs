using Unity.Entities;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{
    private StateEventSystem stateEventSystem;
    private AnimationEventSystem animationEventSystem;

    private InteractableEventSystem interactableEventSystem;

    private SoundEventSystem soundEventSystem;
    private VisualEventSystem visualEventSystem;

    private DropSystem dropSystem;
    private LootSystem lootSystem;
    private InvincibleSystem invincibleSystem;

    private UISystem uiSystem;

    private GlobalEventListenerSystem globalEventListenerSystem;
    private CleanupSystem cleanupSystem;
    private PlayerCollisionSystem playerCollisionSystem;

    private InteractableDoorSystem interactableDoorSystem;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        stateEventSystem = world.GetOrCreateSystem<StateEventSystem>();
        animationEventSystem = world.GetOrCreateSystem<AnimationEventSystem>();

        interactableEventSystem = world.GetOrCreateSystem<InteractableEventSystem>();

        soundEventSystem = world.GetOrCreateSystem<SoundEventSystem>();
        lootSystem = world.GetOrCreateSystem<LootSystem>();
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();
        cleanupSystem = world.GetOrCreateSystem<CleanupSystem>();
        dropSystem = world.GetOrCreateSystem<DropSystem>();
        uiSystem = world.GetOrCreateSystem<UISystem>();
        globalEventListenerSystem = world.GetOrCreateSystem<GlobalEventListenerSystem>();
        playerCollisionSystem = world.GetOrCreateSystem<PlayerCollisionSystem>();
        invincibleSystem = world.GetOrCreateSystem<InvincibleSystem>();
        interactableDoorSystem = world.GetOrCreateSystem<InteractableDoorSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();

        presentation.AddSystemToUpdateList(stateEventSystem);
        presentation.AddSystemToUpdateList(animationEventSystem);
        presentation.AddSystemToUpdateList(interactableEventSystem);
        presentation.AddSystemToUpdateList(lootSystem);
        presentation.AddSystemToUpdateList(visualEventSystem);
        presentation.AddSystemToUpdateList(soundEventSystem);
        presentation.AddSystemToUpdateList(cleanupSystem);
        presentation.AddSystemToUpdateList(dropSystem);
        presentation.AddSystemToUpdateList(uiSystem);
        presentation.AddSystemToUpdateList(globalEventListenerSystem);
        presentation.AddSystemToUpdateList(invincibleSystem);
        presentation.AddSystemToUpdateList(interactableDoorSystem);
        // presentation.AddSystemToUpdateList(playerCollisionSystem);

        presentation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        stateEventSystem.Update();
        animationEventSystem.Update();

        interactableEventSystem.Update();

        uiSystem.Update();
        dropSystem.Update();
        lootSystem.Update();
        soundEventSystem.Update();
        visualEventSystem.Update();

        globalEventListenerSystem.Update();
        cleanupSystem.Update();
        invincibleSystem.Update();
        // playerCollisionSystem.Update();

        interactableDoorSystem.Update();
    }

    public void OnSwapLevel()
    {
    }
}