

const initState = {
    events: [],
    searched: [],
}

const eventsReducer = (state=initState,action) => {
    switch(action.type)
    {
        case "FETCH_EVENTS":
            return {...state, 
                events: action.payload.events}
        default:
            return {...state}
    }
}

export default eventsReducer;