using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.Player
{
    public class PlayerEquipment : MonoBehaviour
    {
        [SerializeField] private EquipmentPositionData[] _positionDatas;

        [System.Serializable]
        public class EquipmentPositionData
        {
            [SerializeField] private EquipmentPosition _type;
            [SerializeField] private Transform _position;

            public EquipmentPosition Type => _type;
            public Transform Position => _position;
        }

        private Dictionary<EquipmentPosition, GameObject> _equipmentObject = new ();

        public void Equip(EquipmentPosition positionType, GameObject equipment)
        {
            //既に装備済みではないか調べる
            if(_equipmentObject.TryGetValue(positionType, out var obj) && obj != null)
            {
                Debug.LogError($"[PlayerEquipment] 二重装備をしようとしました。Position:{positionType} Equipment:{equipment}");
                return;
            }

            var position = GetPosition(positionType);
            //親子関係にし、同じ位置向きにする
            equipment.transform.parent = position;
            equipment.transform.localPosition = Vector3.zero;
            equipment.transform.localRotation = Quaternion.identity;

            //装備品を保存する
            _equipmentObject[positionType] = equipment;
        }

        public GameObject Remove(EquipmentPosition positionType)
        {
            //装備品があるか調べる
            if (!_equipmentObject.TryGetValue(positionType, out var equipment) && equipment == null)
            {
                Debug.LogError($"[PlayerEquipment] 装備がありません。Position:{positionType}");
                return　null;
            }

            //親子関係を解除する
            equipment.transform.parent = null;

            //登録解除
            _equipmentObject[positionType] = null;

            return equipment;
        }

        private Transform GetPosition(EquipmentPosition type)
        {
            var data = _positionDatas.FirstOrDefault(x => x.Type == type);
            if(data == null)
            {
                Debug.LogError($"[PlayerEquipment]　装備位置が見つかりません。 type:{type}");
                return null;
            }

            return data.Position;
        }
    }

    public enum EquipmentPosition
    {
        Overhead
    }
}
