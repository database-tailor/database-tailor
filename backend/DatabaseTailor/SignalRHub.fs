module SignalRHub

open System.Threading.Tasks
open System
open Microsoft.AspNetCore.SignalR

type CursorMovedEvent = {
  PosX: int
  PosY: int
  Username: string
}

type TableMovedEvent = {
  TableId: Guid
  PosX: int
  PosY: int
}

type TableRenamedEvent = {
  TableId: Guid
  TableName: string
}

type ColumnAddedEvent = {
  TableId: Guid
  ColumnIndex: int
  ColumnName: string
  ColumnType: string
}

type ColumnRemovedEvent = {
  TableId: Guid
  ColumnIndex: int
}

type UserJoinedEvent = {
  Username: string
  SessionId: Guid
}

type UserDisconnectedEvent = {
  Username: string
  SessionId: Guid
}

type IClientHub =
  abstract CursorMoved: CursorMovedEvent -> Task
  abstract TableMovedEvent: TableMovedEvent -> Task
  abstract TableRenamedEvent: TableRenamedEvent -> Task
  abstract ColumnAdded: ColumnAddedEvent -> Task
  abstract ColumnRemoved: ColumnRemovedEvent -> Task
  abstract UserJoined: UserJoinedEvent -> Task
  abstract UserDisconnected: UserDisconnectedEvent -> Task

type SignalRHub() =
  inherit Hub<IClientHub>()
