	String.prototype._dhx_trim = function(){
                     return this.replace(/&nbsp;/g," ").replace(/(^[ \t\n\r]*)|([ \t\n\r]*$)/g,"");
                  }
		/* get node as incoming parameter */
		var treeNode1=null;
		var t=document.location.href.split("?");
		var type01=null;
		var base=t[0].replace("/index.s?html","");
		if(t[1]!=null){
			var u=t[1].split("&");
			for(var q=0;q<u.length;q++){
				if((treeNode1==null)&&(u[q].split("=")[0]=="treeNode1")){
					treeNode1=(u[q].split("=")[1]!=null?u[q].split("=")[1]:null);
					if(treeNode1!=null){
						if(treeNode1.length==0){
							treeNode1=null;
						}
					};
				}
				if((type01==null)&&(u[q].split("=")[0]=="type01")){
					type01=(u[q].split("=")[1]!=null?u[q].split("=")[1]:null);
				}
			};
		};

		
		/* open path funtion */
		
		function openPathExamples(itemId){
			//debugger;
			var url = (tree_smpl.getUserData(itemId, "url")!=null?tree_smpl.getUserData(itemId, "url").toString()._dhx_trim():"");
			if (url=="" && !tree_smpl.hasChildren(itemId)) { 
				url = url+"#"+itemId; 
			}
			var itemIdTmp = itemId;
			var i = 0;
			do {
				itemIdTmp = tree_smpl.getParentId(itemIdTmp);
				if (tree_smpl.getUserData(itemIdTmp, "url") != null) { 
					url = tree_smpl.getUserData(itemIdTmp, "url").toString()._dhx_trim() + ((url.indexOf("#")!==0)?"/":"") + url; 
				}
				i++;
			} while (itemIdTmp != 0)
			window.frames.contentFrame.location.href = url+"?un="+Date.parse(new Date());
		}
		
		function openPathDocs(id){
		    var menuId = id;
		    var sUrlMenuLocation="";
			if(tree_menu.getUserData(id,"thisurl")!=null){
			    sUrlMenuLocation = tree_menu.getUserData(id,"thisurl");
			    if(sUrlMenuLocation && sUrlMenuLocation != null && sUrlMenuLocation.replace(/(^\s*)|(\s*$)/g,"") != "")
			    {
			        if(sUrlMenuLocation.indexOf("?")>0){
			            sUrlMenuLocation = sUrlMenuLocation + "&rnd=" + Math.random();
			        }else{
			            sUrlMenuLocation = sUrlMenuLocation + "?rnd=" + Math.random();
			        }
				    window.frames.contentFrame.location.href = sUrlMenuLocation;
				}
				return;
			}
			var entUrl = "";
			var getFileFl = true;
			var suffix = "#"+id
			
//			do{
//				var url = tree_menu.getUserData(id,"url");
//				if(url!=null){
//					if(getFileFl){
//						entUrl = url.toString()._dhx_trim()+suffix;
//						getFileFl = false;
//					}else{
//						var arTmp = url.split("/");
//						if(arTmp[arTmp.length-1].indexOf(".")!=-1){
//							arTmp[arTmp.length-1] = "";
//							url = arTmp.join("/");
//						}
//						if(url!="")
//							entUrl = url.toString()._dhx_trim()+"/"+entUrl;
//					}
//				}
//				id = tree_menu.getParentId(id);
//			}while(id!="0"){
//			    sUrlMenuLocation = entUrl.replace(suffix,"");
//			    if(sUrlMenuLocation && sUrlMenuLocation != null && sUrlMenuLocation.replace(/(^\s*)|(\s*$)/g,"") != "")
//			    {
//			        if(sUrlMenuLocation.indexOf("?")>0){
//			            sUrlMenuLocation = sUrlMenuLocation + "&rnd=" + Math.random();
//			        }else{
//			            sUrlMenuLocation = sUrlMenuLocation + "?rnd=" + Math.random();
//			        }
//			        sUrlMenuLocation += suffix;
//			        window.frames.contentFrame.location.href = sUrlMenuLocation;
//				}
//			    if(window.frames.topContentFrame!=null){
//			        window.frames.topContentFrame.location.href = "actionPath.aspx?menuId="+menuId;
//			    }
//			}
			var url = tree_menu.getUserData(id,"url");
			if(url!=null){
			    entUrl = url.toString()._dhx_trim()+suffix;
		        sUrlMenuLocation = entUrl.replace(suffix,"");
		        if(sUrlMenuLocation && sUrlMenuLocation != null && sUrlMenuLocation.replace(/(^\s*)|(\s*$)/g,"") != "")
		        {
		            if(sUrlMenuLocation.indexOf("?")>0){
		                sUrlMenuLocation = sUrlMenuLocation + "&rnd=" + Math.random();
		            }else{
		                sUrlMenuLocation = sUrlMenuLocation + "?rnd=" + Math.random();
		            }
		            sUrlMenuLocation += suffix;
		            window.frames.contentFrame.location.href = sUrlMenuLocation;
			    }
		        if(window.frames.topContentFrame!=null){
		            window.frames.topContentFrame.location.href = "actionPath.aspx?menuId="+menuId;
		        }
		    }
		}
		
		function updateTreeSize(){
			this.allTree.style.overflow = "visible";
			this.allTree.style.height = this.allTree.scrollHeight+"px";
			
		}
		
		function autoselectNode(){
			if(type01=="smpl"){
				tree_smpl.selectItem(treeNode1,true);tree_smpl.openItem(treeNode1)
			}else{
				tree_menu.selectItem(treeNode1,true);tree_menu.openItem(treeNode1)
			}
		} 