import React from 'react';
import PropTypes from 'prop-types';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";

const YoutubeEmbed = ({embedId}) => {
    return (
        <Responsive>
            <iframe
                width="480"
                height="480"
                src={`https://www.youtube.com/embed/${embedId}`}
                frameBorder="0"
                allow="acceleromter; clipboard-write; encrypted-media; gyroscope; picture-in-picture;"
                allowFullScreen
                title="test"
            />
        </Responsive>
    );
};

YoutubeEmbed.propTypes = {
    embedId: PropTypes.string.isRequired
};

const Responsive = styled(motion.div)`
    overflow: hidden;
    padding-bottom: 56.25%;
    position: relative;
    height: 0;

    iframe{
        left: 0;
        top: 0;
        height: 100%;
        width: 100%;
        position: absolute;
    }
`;

export default YoutubeEmbed;