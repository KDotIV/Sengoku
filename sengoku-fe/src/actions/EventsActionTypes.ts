export const EVENTS_LOADING = "EVENTS_LOADING";
export const EVENTS_FAIL = "EVENTS_FAIL";
export const EVENTS_SUCCESS = "EVENTS_SUCCESS";

export type EventType = {
    address: EventAddress
    name: EventName
    status: EventStatus
    event_id: EventId
    city: EventCity
    game: EventGame
}

export type EventAddress = {
    building: string
    street: string
    zipcode: string
}
export type EventName = {
    name: string
}
export type EventStatus = {
    status: string
}

export type EventId = {
    event_Id: string
}

export type EventCity = {
    city: string
}

export type EventGame = {
    game: string
}

export interface EventsLoading {
    type: typeof EVENTS_LOADING
}

export interface EventsFail {
    type: typeof EVENTS_FAIL
}

export interface EventsSuccess {
    type: typeof EVENTS_SUCCESS
    payload: EventType
}

export type EventDispatchTypes = EventsLoading | EventsFail | EventsSuccess