import React from 'react';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";

const Events = ({ eventName, eventLocation, eventGame, eventAddress }) => {
    return(
        <CardStyle>
            <h3>{eventName}</h3>
            <p>{eventLocation}</p>
            <p>{eventGame}</p>
        </CardStyle>
    );
}

const CardStyle = styled(motion.div)`
    min-height:30vh;
    box-shadow: 0px 5px 20px rgba(0,0,0,0.2);
    text-align: center;
    border-radius: 1rem;
    img{
        width: 100%;
        height: 40vh;
        object-fit: cover;
    }
`;

export default Events;
