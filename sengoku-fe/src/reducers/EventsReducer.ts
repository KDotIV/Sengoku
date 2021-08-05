import { EventDispatchTypes, EventType, EVENTS_SUCCESS, EVENTS_FAIL, EVENTS_LOADING } from "../actions/EventsActionTypes";

interface DefaultStateI {
    loading: boolean,
    eventResult?: EventType
}

const defaultState: DefaultStateI = {
    loading: false
};

const eventsReducer = (state : DefaultStateI = defaultState, action: EventDispatchTypes) : DefaultStateI => {
    switch (action.type) {
        case EVENTS_FAIL:
            return {
                loading: false,
            }
        case EVENTS_LOADING:
            return {
                loading: true,
            }
        case EVENTS_SUCCESS:
            return {
                loading: false,
                eventResult: action.payload
            }
        default:
            return state
    }
}

export default eventsReducer