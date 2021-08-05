import { Dispatch } from "redux"
import { EventDispatchTypes, EVENTS_SUCCESS, EVENTS_FAIL, EVENTS_LOADING } from "./EventsActionTypes";
import axios from "axios";

export const GetEvents = (eventId: string ) => async (dispatch: Dispatch<EventDispatchTypes>) => {
    try {
        dispatch({
            type: EVENTS_LOADING
        })

        const res = await axios.get(`https://localhost:5001/api/events/GetEventByID/${eventId}`)

        dispatch({
            type: EVENTS_SUCCESS,
            payload: res.data
        })

    } catch (e) {
        dispatch({
            type: EVENTS_FAIL
        })
    }
}