namespace LuckyPills
{
    using System;
    using Exiled.API.Features;
    using PlayerEvent = Exiled.Events.Handlers.Player;
    
    public class Plugin : Plugin<Config, Translation>
    {
        public override string Author { get; } = "RAPLX";
        public override string Name { get; } = "Lucky Pills";
        public override Version Version { get; } = new Version(1, 1, 1);
        public override Version RequiredExiledVersion { get; } = new Version(8, 0, 0);

        private EventHandlers eventHandler;

        public override void OnEnabled()
        {
            eventHandler = new EventHandlers(this);

            PlayerEvent.UsedItem += eventHandler.UsedItem;
            PlayerEvent.PickingUpItem += eventHandler.OnPickup;
            
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            PlayerEvent.UsedItem -= eventHandler.UsedItem;
            PlayerEvent.PickingUpItem -= eventHandler.OnPickup;

            eventHandler = null;
            
            base.OnDisabled();
        }
    }
}
