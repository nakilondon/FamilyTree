import { connect } from 'react-redux'
import { setViewMode } from '../actions'
import Link from '../components/Link'

const mapStateToProps = (state, ownProps) => {
  return {
    active: ownProps.filter === state.viewMode,
    value: ownProps.filter
  }
}

const mapDispatchToProps = (dispatch, ownProps) => {
  return {
    onClick: () => {
      dispatch(setViewMode(ownProps.filter))
    }
  }
}

const ViewLink = connect(mapStateToProps, mapDispatchToProps)(Link)

export default ViewLink