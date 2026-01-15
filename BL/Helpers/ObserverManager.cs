namespace Helpers;

/// <summary>
/// Manages observer registration and notification for business entities.
/// Supports both list-level and entity-level observers.
/// </summary>
class ObserverManager
{
    private event Action? _listObservers;
    private readonly Dictionary<int, Action?> _specificObservers = new();

    /// <summary>
    /// Registers an observer to be notified when the entity list changes.
    /// </summary>
    internal void AddListObserver(Action observer) => _listObservers += observer;

    /// <summary>
    /// Unregisters an observer from list change notifications.
    /// </summary>
    internal void RemoveListObserver(Action observer) => _listObservers -= observer;

    /// <summary>
    /// Registers an observer for changes to a specific entity instance.
    /// </summary>
    internal void AddObserver(int id, Action observer)
    {
        if (_specificObservers.ContainsKey(id))
            _specificObservers[id] += observer;
        else
            _specificObservers[id] = observer;
    }

    /// <summary>
    /// Unregisters an observer from a specific entity instance.
    /// </summary>
    internal void RemoveObserver(int id, Action observer)
    {
        if (_specificObservers.ContainsKey(id) && _specificObservers[id] is not null)
        {
            Action? specificObserver = _specificObservers[id];
            specificObserver -= observer;

            if (specificObserver?.GetInvocationList().Length == 0)
                _specificObservers.Remove(id);
        }
    }

    /// <summary>
    /// Notifies all list observers about a change affecting the entity list.
    /// </summary>
    internal void NotifyListUpdated() => _listObservers?.Invoke();

    /// <summary>
    /// Notifies observers of a specific entity about a change.
    /// </summary>
    internal void NotifyItemUpdated(int id)
    {
        if (_specificObservers.ContainsKey(id))
            _specificObservers[id]?.Invoke();
    }
}
