using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using VoxDOTS.Components;
using VoxDOTS.Data;
using VoxDOTS.Tags;

namespace VoxDOTS.Systems
{
    public class ChunkSpawnerTest : MonoBehaviour
    {
        public uint3 worldSize;
        public bool buildChunks;
        public int maxChunksPerTick = 1;

        private bool _isUpdating;
        private int _currentPosition;
        private EntityManager _entityManager;
        private EntityArchetype _entityArchetype;

        private async void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            _entityArchetype = _entityManager.CreateArchetype(
                typeof(Chunk),
                typeof(ChunkData),
                typeof(ChunkMeshVerticesData),
                typeof(ChunkMeshIndicesData),
                typeof(ChunkDataNoiseGeneratorTag),
                typeof(ChunkNeedsVoxelDataTag)
            );

            //await Task.Delay(5000);

            //buildChunks = true;
        }

        private void Update()
        {
            if (buildChunks)
            {
                Debug.Log("Go");
                buildChunks = false;

                CreateChunks();

                _isUpdating = true;

                _currentPosition = 0;
                return;
            }

            if (_isUpdating)
                CreateChunks();
        }

        private void CreateChunks()
        {
            var i = 0;
            
            while(_currentPosition < (worldSize.x * worldSize.y * worldSize.z))
            {
                if (i >= maxChunksPerTick)
                {
                    return;
                }

                var x = (uint)_currentPosition % worldSize.x;
                var y = (uint)(_currentPosition / worldSize.x) % worldSize.y;
                var z = (uint)(_currentPosition / worldSize.x / worldSize.y) % worldSize.z;

                CreateChunkEntity(new uint3(x * Constants.ChunkSize, y * Constants.ChunkSize, z * Constants.ChunkSize));
                i++;
                _currentPosition++;
            }

            _isUpdating = false;
        }

        private Entity CreateChunkEntity(uint3 position)
        {
            var chunkEntity = _entityManager.CreateEntity(_entityArchetype);

            var buffer = _entityManager.GetBuffer<ChunkData>(chunkEntity);
            buffer.ResizeUninitialized(Constants.MaxChunkSize);
            
            _entityManager.SetComponentData(chunkEntity, new Chunk
            {
                Position = position
            });

            return chunkEntity;
        }
    }
}