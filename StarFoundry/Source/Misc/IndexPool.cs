using System;
using System.Collections.Generic;
using StarFoundry.ECS;

namespace StarFoundry.Misc;

/// <summary>
/// A pool of integer indices that can be borrowed and freed. The maximum index is resized automatically when necessary
/// and the pool can be grown manually with <see cref="Grow"/>. You can subscribe to <see cref="IndexPoolResized"/> to
/// deal with any changes.<br /><br />
/// This class is used in <see cref="Universe{TEntity}"/> to keep track of free indices for entities.
/// </summary>
public class IndexPool {
    public event Action<int, int> IndexPoolResized = delegate { };

    // We use a queue of free indices combined with a minimum unused index to keep track of free indices.
    // The queue stores only the indices that have been borrowed and freed (i.e. less than the minimum unused index).
    private readonly Queue<int> _freeIndices;
    private int _minUnusedIndex;
    private int _maxIndex;

    public IndexPool(int initialSize = 64) {
        _freeIndices = new Queue<int>(initialSize);
        _minUnusedIndex = 0;
        _maxIndex = initialSize;
    }

    /// <summary>
    /// Borrows an index from the pool. If there are no free indices, the pool will be resized automatically.
    /// </summary>
    public int Borrow() {
        if (_freeIndices.Count > 0) return _freeIndices.Dequeue();
        if (_minUnusedIndex >= _maxIndex) Grow((int)(_maxIndex * 1.5));

        return _minUnusedIndex++;
    }

    /// <summary>
    /// Frees an index that was previously borrowed to be reused later.
    /// </summary>
    public void Free(int index) {
        if (index == _minUnusedIndex - 1) _minUnusedIndex--;
        else if (index < _minUnusedIndex) _freeIndices.Enqueue(index);
        else {
            throw new ArgumentOutOfRangeException(nameof(index),
                "Cannot free an index that is larger than the minimum unused index.");
        }
    }

    /// <summary>
    /// Grows the pool to the given size. If the size is smaller than the maximum index, an exception will be thrown.
    /// </summary>
    public void Grow(int size) {
        if (size < _maxIndex)
            throw new ArgumentOutOfRangeException(nameof(size),
                "Cannot grow to a size smaller than the maximum index.");

        _maxIndex = size;
        IndexPoolResized(_minUnusedIndex, _maxIndex);
    }

    /// <summary>
    /// Shrinks the pool to the given size. If the size is smaller than the minimum unused index, an exception will be
    /// thrown.
    /// </summary>
    public void Shrink(int size) {
        if (size < _minUnusedIndex)
            throw new ArgumentOutOfRangeException(nameof(size),
                "Cannot shrink to a size smaller than the minimum unused index.");

        if (size > _maxIndex)
            throw new ArgumentOutOfRangeException(nameof(size),
                "Cannot shrink to a size larger than the maximum index.");

        _maxIndex = size;
        IndexPoolResized(_minUnusedIndex, _maxIndex);
    }
}