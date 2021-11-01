import React from 'react';
import PropTypes from 'prop-types';
//Styling and Animation
import styled from 'styled-components';
import { motion } from "framer-motion";

const YoutubeEmbed = ({embedId}) => {

    if(validateYouTubeUrl(embedId))
    {
        return (
            <Responsive>
                <iframe
                    src={`https://www.youtube.com/embed/${embedId}`}
                    frameBorder="0"
                    allowFullScreen
                    title="test"
                />
            </Responsive>
        );
    } else {
        return null;
    }
};

function validateYouTubeUrl(embedId)
{
    const url = "https://www.youtube.com/embed/" + embedId;
    if (url !== undefined || url !== '') {
        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=|\?v=)([^#\&\?]*).*/;
        var match = url.match(regExp);
        if (match && match[2].length === 11) {
            return true;
        }
        else {
            return false;
        }
    }
}

YoutubeEmbed.propTypes = {
    embedId: PropTypes.string.isRequired
};

const Responsive = styled(motion.div)`
    display: flex;
    overflow: hidden;
    position: relative;
    height: 500px;
    width: 720px;

    ::after{
        clear: both;
    }
    iframe{
        left: 0;
        top: 0;
        height: 100%;
        width: 100%;
        position: absolute;
    }
`;

export default YoutubeEmbed;