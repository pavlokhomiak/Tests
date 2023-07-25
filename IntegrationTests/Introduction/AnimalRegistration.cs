namespace IntegrationTests.Introduction;

public class AnimalRegistration
{
    private readonly AppDbContext _context;
    private readonly IEventSink _eventSink;
    private readonly ISystemClock _systemClock;
    
    public AnimalRegistration(AppDbContext context, IEventSink eventSink, ISystemClock systemClock)
    {
        _context = context;
        _eventSink = eventSink;
        _systemClock = systemClock;
    }

    public async Task Register(Animal animal)
    {
        animal.Created = _systemClock.Now();
        _context.Animals.Add(animal);
        await _context.SaveChangesAsync();
        
        _eventSink.SendEmailsToCustomerThatWantThisTypeOfAnimalsAndAttachPictures();
    }
}