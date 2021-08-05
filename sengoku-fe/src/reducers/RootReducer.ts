import { combineReducers } from "redux";
import eventsReducer from "./EventsReducer";

const RootReducer = combineReducers({
  events: eventsReducer
});

export default RootReducer