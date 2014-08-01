﻿using System;

namespace Core
{
    public enum PacketType
    {
        BasePlayerCreate = 0x00,
        CellPlayerCreate = 0x01,
        EntityControl = 0x02,
        EntityEnter = 0x03,
        EntityLeave = 0x04,
        EntityCreate = 0x05,
        EntityProperties = 0x06,
        EntityProperty = 0x07,
        EntityMethod = 0x08,
        EntityMove = 0x09,
        EntityMoveWithError = 0x0A,
        SpaceData = 0x0B,
        SpaceGone = 0x0C,
        StreamComplete = 0x0D,
        EntitiesReset = 0x0E,
        RestoreClient = 0x0F,
        EnableEntitiesRejected = 0x10,
        ClientReady = 0x11,
        SetArenaPeriod = 0x12,
        SetArenaLength = 0x13,
        ClientVersion = 0x14,
        UpdateCamera = 0x15,
        UpdateGunMarker = 0x16,
        ChangeControlMode = 0x17,
        UpdateTurretYaw = 0x18,
        UpdateGunPitch = 0x19,
        AmmoButtonPressed = 0x1A,
        UpdateFpspinglag = 0x1B,
        SetGunReloadTime = 0x1C,
        SetActiveConsumableSlot = 0x1D,
        SetPlayerVehicleId = 0x1E,
        BattleChatMessage = 0x1F,
        NestedEntityProperty = 0x20,
        MinimapCellClicked = 0x21,
        UpdateCamera2 = 0x22,
        SetServerTime = 0x23,
        LockTarget = 0x24,
        SetCruiseMode = 0x25,
    }
}
