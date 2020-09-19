
function GetBookDataJson(IdBook, ChapterBook) {
    $.ajax({
                    type: "GET",
                    url: '/Books/GetDataBook',

                    data: {
                     page: ChapterBook, idBook: IdBook
                    },

                    success: function (data) {
                        
                        $("#content").html(data);
                        
                        $("#opt :nth-child(" + (Number(ChapterBook) + 1)+")").attr("selected", "selected");
                        $('html, body').animate({
                            scrollTop: $("html").offset().top
                        });
                    }
                });

}

