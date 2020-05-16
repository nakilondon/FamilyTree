export const setViewMode = filter => ({
    type: 'SET_VIEW_MODE_FILTER',
    filter
})

export const ViewMode = {
    SHOW_FAMILY_TREE: 'SHOW_FAMILY_TREE',
    SHOW_DETAIL: 'SHOW_DETAIL',
    SHOW_EDIT: 'SHOW_EDIT',
    SHOW_UPLOAD: 'SHOW_UPLOAD'
}