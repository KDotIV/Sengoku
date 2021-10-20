import React from 'react';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";
//Redux
import { useDispatch } from 'react-redux';
import { getEvent } from '../actions/eventsAction';
import { Link } from 'react-router-dom';

const Events = ({ eventName, eventLocation, eventGame, eventAddress, id }) => {
    //GetEvent
    const dispatch = useDispatch();
    const getEventHandler = () => {
        dispatch(getEvent(id))
    };

    return(
        <CardStyle onClick={getEventHandler}>
            <Link to={`/event/${id}`}>
            <h3>{eventName}</h3>
            <p>{eventLocation}</p>
            <p>{eventGame}</p>
            </Link>
        </CardStyle>
    );
}

const CardStyle = styled(motion.div)`
    min-height:30vh;
    box-shadow: 0px 5px 20px rgba(0,0,0,0.2);
    text-align: center;
    border-radius: 1rem;
    cursor: pointer;
    img{
        width: 100%;
        height: 40vh;
        object-fit: cover;
    }
`;

export default Events;
