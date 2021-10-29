const initState = {
    allLegends: [{}],
    searched: [{}],
    legend: {plotPoints: []},
    isLoading: true,
}

const legendsReducer = (state = initState,action) => {
    switch(action.type)
    {
        case "FETCH_LEGENDS":
            return {...state, 
                allLegends: action.payload.legendResult,
                isLoading: false,
            };
        case "GET_LEGEND":
            return {...state,
                legend: action.payload.legendResult,
                isLoading: false,
            };
        case "LOADING_LEGENDS":
            return {...state,
                isLoading: true,
            };
        default:
            return {...state};
    }
}

export default legendsReducer;
