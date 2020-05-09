import React from 'react'
import PropTypes from 'prop-types'
import ToggleButton from '@material-ui/lab/ToggleButton';

const Link = ({ value, active, children, onClick }) => (
  <ToggleButton
    value={value}
    onClick={onClick}
    disabled={active}
    style={{
      marginLeft: '4px'
    }}
  >
    {children}
  </ToggleButton>
)

Link.propTypes = {
  active: PropTypes.bool.isRequired,
  children: PropTypes.node.isRequired,
  onClick: PropTypes.func.isRequired,
  value: PropTypes.string.isRequired
}

export default Link